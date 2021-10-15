import random
import string

def Generate(length, digits = False, assci_upper = False, assci_lower = False, symbols = False):
    letters = ""

    if(digits):
        letters += string.digits
    if(assci_upper):
        letters += string.ascii_uppercase
    if(assci_lower):
        letters += string.ascii_lowercase
    if(symbols):
        letters += string.punctuation
        
    if(letters==""):
        return "*" * length
    else:
        return ''.join(random.choice(letters) for i in range(length))