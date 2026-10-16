VAR Track1 = false

-> main

=== main ===
We have two music options to choose from, composed by two highly talented individuals: Maria and Pablo.  #speaker: Goldfish
Whose music would you like to listen to? #speaker: Goldfish
    *[Maria!]
        Great choice! #speaker: Goldfish
        ~Track1 = true
        -> END
    *[Pablo!]
        Here you go! #speaker: Goldfish
        ~Track1 = false
        -> END
-> END