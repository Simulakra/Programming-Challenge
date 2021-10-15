import StringGenerator

str_len = input("Enter password length:")

digits = True
assci_upper = True
assci_lower = True
symbols = True

a = StringGenerator.Generate(int(str_len))
b = StringGenerator.Generate(int(str_len), digits, assci_upper, assci_lower, symbols)
print(a)
print(b)