 -> main
 
 === main ===
 I'm waiting for today's gift, something delicious
 I can feel it,
    + [Umm... some fish?]
        -> chosen("Umm... some fish?")
    + [A pants?]
        -> chosen("A pants?")
    + [I don't think there will have any gift.]
        -> chosen("I don't think there will have any gift.")
        
=== chosen(Motivation) ===
You think I want some {Motivation}?
-> END