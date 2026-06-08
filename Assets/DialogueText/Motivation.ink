 -> main
 
 === main ===
 What you want to do, Bobby?
    + [Follow me]
        -> chosen("Follow me")
    + [Dancing]
        -> chosen("Dancing")
    + [Nothing]
        -> chosen("Nothing")
        
=== chosen(Motivation) ===
You chose {Motivation}!
-> END