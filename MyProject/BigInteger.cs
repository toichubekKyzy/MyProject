using System;
using System.Text;
using System.Text.RegularExpressions;

public interface IArithmeticOperations
{
    BigInteger Add(BigInteger other);
    BigInteger Subtract(BigInteger other);
    BigInteger Multiply(BigInteger other);
    BigInteger Divide(BigInteger other);
}

public class BigInteger : IArithmeticOperations
{
    private string value;

    public BigInteger(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Некорректный ввод");
        }

        this.value = value;
    }

    public BigInteger Add(BigInteger other)
    {
        string result = PerformAddition(this.value, other.value);
        return new BigInteger(result);
    }

    public BigInteger Subtract(BigInteger other)
    {
        string result = PerformSubtraction(this.value, other.value);
        return new BigInteger(result);
    }

    public BigInteger Multiply(BigInteger other)
    {
        string result = PerformMultiplication(this.value, other.value);
        return new BigInteger(result);
    }

    public BigInteger Divide(BigInteger other)
    {
        if (other.IsZero())
        {
            throw new DivideByZeroException("Деление на ноль");
        }

        string result = PerformDivision(this.value, other.value);
        return new BigInteger(result);
    }

    private bool IsValid(string value)
    {
        string pattern = @"^-?\d+$";
        return Regex.IsMatch(value, pattern);
    }

    private bool IsZero()
    {
        return this.value == "0";
    }

    private string PerformAddition(string num1, string num2)
    {
        int maxLength = Math.Max(num1.Length, num2.Length);
        num1 = num1.PadLeft(maxLength, '0');
        num2 = num2.PadLeft(maxLength, '0');

        StringBuilder result = new StringBuilder();
        int carry = 0;

        for (int i = maxLength - 1; i >= 0; i--)
        {
            int digit1 = num1[i] - '0';
            int digit2 = num2[i] - '0';

            int sum = digit1 + digit2 + carry;
            carry = sum / 10;
            result.Insert(0, sum % 10);
        }

        if (carry > 0)
        {
            result.Insert(0, carry);
        }

        return result.ToString();
      
    }

    private string PerformSubtraction(string num1, string num2)
    {
        int maxLength = Math.Max(num1.Length, num2.Length);
        num1 = num1.PadLeft(maxLength, '0');
        num2 = num2.PadLeft(maxLength, '0');

        StringBuilder result = new StringBuilder();
        int borrow = 0;

        for (int i = maxLength - 1; i >= 0; i--)
        {
            int digit1 = num1[i] - '0' - borrow;
            int digit2 = num2[i] - '0';

            if (digit1 < digit2)
            {
                digit1 += 10;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }

            result.Insert(0, digit1 - digit2);
        }

        // Убираем ведущие нули
        while (result.Length > 1 && result[0] == '0')
        {
            result.Remove(0, 1);
        }

        return result.ToString();
       
    }

    private string PerformMultiplication(string num1, string num2)
    {
        int len1 = num1.Length;
        int len2 = num2.Length;

        // Результирующая строка, инициализируемая нулями
        int[] resultArray = new int[len1 + len2];

        // Умножение цифр в обратном порядке
        for (int i = len1 - 1; i >= 0; i--)
        {
            for (int j = len2 - 1; j >= 0; j--)
            {
                int product = (num1[i] - '0') * (num2[j] - '0');

                // Добавляем произведение к соответствующему разряду результата
                resultArray[i + j + 1] += product;

                // Обрабатываем переносы
                if (resultArray[i + j + 1] >= 10)
                {
                    resultArray[i + j] += resultArray[i + j + 1] / 10;
                    resultArray[i + j + 1] %= 10;
                }
            }
        }

        // Преобразуем массив в строку
        StringBuilder result = new StringBuilder();
        foreach (int digit in resultArray)
        {
            result.Append(digit);
        }

        // Убираем ведущие нули
        while (result.Length > 1 && result[0] == '0')
        {
            result.Remove(0, 1);
        }

        return result.ToString();
    }

    private string PerformDivision(string num1, string num2)
    {
        if (num2 == "0")
        {
            throw new DivideByZeroException("Деление на ноль");
        }

        if (num1 == "0" || string.Compare(num1, num2) < 0)
        {
            return "0";
        }

        int len1 = num1.Length;
        int len2 = num2.Length;

        StringBuilder quotient = new StringBuilder();
        string remainder = num1.Substring(0, len2);

        int currentIndex = len2;

        while (currentIndex <= len1)
        {
            int currentQuotientDigit = 0;

            // Уменьшаем remainder до тех пор, пока он не станет меньше num2
            while (string.Compare(remainder, num2) >= 0)
            {
                remainder = PerformSubtraction(remainder, num2);
                currentQuotientDigit++;
            }

            quotient.Append(currentQuotientDigit);

            if (currentIndex < len1)
            {
                remainder += num1[currentIndex];
                currentIndex++;
            }
        }

        // Убираем ведущие нули в частном
        while (quotient.Length > 1 && quotient[0] == '0')
        {
            quotient.Remove(0, 1);
        }

        return quotient.ToString();
    }

    public static BigInteger operator +(BigInteger left, BigInteger right)
    {
        return left.Add(right);
    }

    public static BigInteger operator -(BigInteger left, BigInteger right)
    {
        return left.Subtract(right);
    }

    public static BigInteger operator *(BigInteger left, BigInteger right)
    {
        return left.Multiply(right);
    }

    public static BigInteger operator /(BigInteger left, BigInteger right)
    {
        return left.Divide(right);
    }

    public override string ToString()
    {
        return value;
    }
}