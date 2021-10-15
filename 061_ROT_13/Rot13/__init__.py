def Convert(keyword, style="codecs"):

    if(style=="codecs"):
        import codecs
        return codecs.encode(keyword, "rot_13")

    elif(style=="string_lib"):
        import string
        rot13 = string.maketrans( 
            "ABCDEFGHIJKLMabcdefghijklmNOPQRSTUVWXYZnopqrstuvwxyz", 
            "NOPQRSTUVWXYZnopqrstuvwxyzABCDEFGHIJKLMabcdefghijklm")
        return string.translate(keyword, rot13)
        
    else:
        return keyword