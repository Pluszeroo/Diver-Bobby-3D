VAR Track1 = false

-> main

=== main ===
Hello, Bobby! Would you like to change the sound of the ocean?  #speaker: Goldfish # emotion: emotion: Singing
    *[Maria!]
        An excellent choice but, of course, they are both excellent choices. Enjoy your groove Bobby! #speaker: Goldfish # emotion: Happy
Thanks Goldfish! #speaker: Bobby
        ~Track1 = true
        -> END
    *[Pablo!]
       An excellent choice but, of course, they are both excellent choices. Enjoy your groove Bobby! #speaker: Goldfish # emotion: Happy
Thanks Goldfish! #speaker: Bobby
        ~Track1 = false
        -> END
-> END