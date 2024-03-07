namespace Utilities.Extensions;

public static class BrazilianDocumentsExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsCPF(this string value)
    {
        int[] m1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] m2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        value = value.DigitsOnly();
            
        if (value.Length != 11) return false;

        for (var i = 0; i < 10; i++)
        {
            if (i.ToString().PadLeft(11, char.Parse(i.ToString())) == value) return false;
        }

        //var temp = value.Substring(0, 9);
        var temp = value[..9];
        var soma = 0;

        for (var i = 0; i < 9; i++)
        {
            soma += int.Parse(temp[i].ToString()) * m1[i];
        }
            
        var resto = soma % 11;
            
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        var digito = resto.ToString();

        temp += digito;
        
        soma = 0;
        
        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(temp[i].ToString()) * m2[i];
        }

        resto = soma % 11;
        
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }
        
        digito += resto.ToString();

        return value.EndsWith(digito);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsCNPJ(this string value)
    {
        int[] m1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] m2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        value = value.DigitsOnly();

        if (value.Length != 14) return false;

        var temp = value[..12];
        int soma = 0;

        for (int i = 0; i < 12; i++)
        {
            soma += int.Parse(temp[i].ToString()) * m1[i];
        }

        var resto = soma % 11;
        
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        var digito = resto.ToString();
        
        temp += digito;
        
        soma = 0;
        
        for (int i = 0; i < 13; i++)
        {
            soma += int.Parse(temp[i].ToString()) * m2[i];
        }

        resto = soma % 11;
        
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        digito += resto.ToString();

        return value.EndsWith(digito);
    }
}
