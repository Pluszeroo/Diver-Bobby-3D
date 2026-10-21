VAR ShowStele = false

-> main

=== main ===
Get the tools you need to break this thing apart and get it out of here. My patience is running thin. #speaker: Scientish
    *[Let's talk!]
        -> convo
    *[Let me see the Stele]
        ~ShowStele = true
        Here you go! #speaker: Scientish
        -> END
-> END

=== convo ===
Hey! #speaker: Bobby
Stop. #speaker: Scientish
-> END