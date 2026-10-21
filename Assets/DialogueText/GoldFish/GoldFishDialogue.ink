VAR Track1 = false

-> main

=== main ===
Wow, you are the most beautiful fish! How do you stay so shiny?  #speaker: Bobby
Hey, I am not just any fish...I am GOLD fish. And it’s all natural, honey.  #speaker: Goldfish # emotion: Happy
You seem almost…magical. Am I staring?! Sorry! My name is Bobby! #speaker: Bobby
That’s okay, Bobby. You are right, I am magical. #speaker: Goldfish # emotion: Happy
I have the power to grant you one wish of your choosing. What will it be? #speaker: Goldfish # emotion: Singing
Anything I want?! A big, BIG house filled with tuna and chicken and toys and… #speaker: Bobby # mood: Mock # emotion: Mock
Slow down, sweetie. I can’t do all that! But I could change the world around you? #speaker: Goldfish 
Hmm. I want to get my groove on. Can you change the music in the ocean? #speaker: Bobby # mood: Begging # emotion: Begging
That I can do! You have two choices of music artists before you... #speaker: Goldfish # emotion: Happy
Will you choose the artist Pablo...or will you choose the artist Mariia? #speaker: Goldfish # emotion: Singing
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