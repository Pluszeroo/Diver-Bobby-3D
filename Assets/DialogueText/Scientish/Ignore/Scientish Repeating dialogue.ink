VAR ShowStele = false

-> main

=== main ===
What can I do for you? #Speaker: Scientish
    *[Let's talk!]
        -> convo
    *[Let me see the Stele]
        ~ShowStele = true
        Here you go! #Speaker: Scientish
        -> END
-> END

=== convo ===
Hey! #speaker: Bobby
Sup! #speaker: Scientish
-> END