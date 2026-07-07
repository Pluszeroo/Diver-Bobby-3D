VAR agreed = false

{agreed: -> already_agreed | -> first_meet}

=== first_meet ===
# speaker: Scientist
This guitar... too big. I cannot move it.
# speaker: Scientist
I heard of tools from the surface that could dismantle it. 
# speaker: Scientist
But I don't know how.
Will you help me?
+ [I'll help you]
    ~ agreed = true
    # speaker: Scientist
    Wonderful! Let me show you the stone tablet...
    -> END
+ [Not my problem]
    # speaker: Scientist
    Oh... well, I'll be here. It's not going anywhere. Neither am I.
    -> END

=== already_agreed ===
# speaker: Scientist
Check the tablet if you forget what we need.
-> END