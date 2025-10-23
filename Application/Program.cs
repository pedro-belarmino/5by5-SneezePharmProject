
Console.Write("Cnpj: ");
string cnpj = Console.ReadLine();



bool ValidarCnpj(string cnpj)
{
    char[] letras = cnpj.ToCharArray();
    int letra1 = int.Parse(letras[0].ToString());
    
    foreach(char l in letras)
    {
        Console.WriteLine(l);
    }


    return true;
}




ValidarCnpj(cnpj);