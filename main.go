package main

import (
	"fmt"
	"log"
	"net/http"

	elec "github.com/asticode/go-astilectron"
	spot "github.com/zmb3/spotify"
)

const redirectURI = "http://localhost:4000/callback"

var (
	auth = spot.NewAuthenticator(redirectURI,
		spot.ScopeUserReadCurrentlyPlaying,
		spot.ScopeUserReadPlaybackState,
		spot.ScopeUserModifyPlaybackState,
		spot.ScopeUserReadBirthdate,
		spot.ScopeUserReadEmail,
		spot.ScopeUserReadPrivate,
		"streaming")
	ch    = make(chan *spot.Client, 1)
	state = "abc123"
)

func main() {

	var client *spot.Client
	var playerState *spot.PlayerState
	var err error

	auth.SetAuthInfo("5481ad94582e4d8c95bc939032e3734f", "ab9ab7790c7741f5872c6e838a44fa20")

	fs := http.FileServer(http.Dir("static"))
	http.Handle("/", fs)

	http.HandleFunc("/callback", completeAuth)
	go func() {
		if err := http.ListenAndServe(":4000", nil); err != nil {
			log.Fatal(err)
		}
	}()

	var a, _ = elec.New(elec.Options{
		AppName: "kookBOX",
	})
	defer a.Close()

	a.Start()

	var w, _ = a.NewWindow(auth.AuthURL(state), &elec.WindowOptions{
		Center: elec.PtrBool(true),
		Height: elec.PtrInt(600),
		Width:  elec.PtrInt(800),
		WebPreferences: &elec.WebPreferences{
			AllowRunningInsecureContent: elec.PtrBool(true),
			Webaudio:                    elec.PtrBool(true),
			Plugins:                     elec.PtrBool(true),
		},
	})

	w.OnMessage(func(m *elec.EventMessage) interface{} {
		log.Print("In OnMessage")
		var s string
		m.Unmarshal(&s)

		if s == "ready" {
			var deviceID spot.ID
			devices, _ := client.PlayerDevices()
			for _, d := range devices {
				if d.Name == "kookBOX" {
					deviceID = d.ID
				}
			}
			log.Printf("%v", deviceID)

			if err = client.TransferPlayback(deviceID, true); err != nil {
				log.Printf("play: %v", err)
			}
		}
		return nil
	})
	w.Create()
	w.OpenDevTools()
	w.Session.ClearCache()

	client = <-ch

	t, _ := client.Token()

	w.SendMessage(t.AccessToken)

	playerState, err = client.PlayerState()
	if err != nil {
		log.Fatal(err)
	}
	log.Printf("Found your %s (%s)\n", playerState.Device.Type, playerState.Device.Name)

	a.Wait()
}

func serveApplication(w http.ResponseWriter, r *http.Request) {
	fmt.Fprintf(w, "Hello")
}

func completeAuth(w http.ResponseWriter, r *http.Request) {

	tok, err := auth.Token(state, r)
	if err != nil {
		http.Error(w, "Couldn't get token", http.StatusForbidden)
		log.Fatal(err)
	}
	if st := r.FormValue("state"); st != state {
		http.NotFound(w, r)
		log.Fatalf("State mismatch: %s != %s\n", st, state)
	}
	http.Redirect(w, r, "http://localhost:4000/main.htm", http.StatusSeeOther)

	client := auth.NewClient(tok)
	ch <- &client
}
