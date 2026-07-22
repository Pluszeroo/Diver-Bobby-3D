VAR agreed = false

{agreed: -> already_agreed | -> first_meet}

=== first_meet ===
# speaker: Scientish
Oh no. No no no. 
# speaker: Scientish
This will not do.
# speaker: Scientish
We cannot have this here. 
# speaker: Scientish
I have run some tests and taken some samples but I just don’t know how to remove it!
# speaker: Scientish
I heard of tools from the surface that could dismantle it. 
# speaker: Scientish
But I don't know how.
Will you help me?
+ [I'll help you]
    ~ agreed = true
    # speaker: Scientish
    Wonderful! Let me show you the stone tablet...
    -> END
+ [Not my problem]
    # speaker: Scientish
    Oh... well, I'll be here. It's not going anywhere. Neither am I.
    -> END

=== already_agreed ===
# speaker: Scientish
- Here, this is the result of my studies. 
# speaker: Scientish
You will be needing this.
-> END