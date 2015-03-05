using System;
using System.Linq;

namespace RocketPos.Common.Helpers
{
	/// <summary>
	/// The idea behind this is the assumption that your validation needs to do repetitive things in multiple
	/// validations (different classes). We move all the duplicated code to a static helper, and simply
	/// call this methods here
	/// </summary>
	public static class Validation_Helper
	{
		/// <summary>
		/// Example of a method to be called repeatedly from a validation method
		/// </summary>
		public static bool ParseInput(string input, bool simpleLogic=true)
		{
			return simpleLogic  ? ParseSimpleLogic (input) 
								: ParseComplexLogic(input);
		}

        /// <summary>
        /// Example of a method to be called repeatedly from a validation method
        /// </summary>
        public static bool ParseDigitInput(string input, bool simpleLogic = true)
        {
            return simpleLogic ? ParseDigitLogic(input)
                                : ParseComplexDigitLogic(input);
        }

		#region Simple or Complex Logic

		/// <summary>
		/// parse a string to see if it contains only digits, white spaces, colons or commas
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool ParseComplexLogic(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return false;   // to avoid null or empty string passing through

			return VerifyValidChars(input, DigitsWhiteSpaceColonOrComma_CharBoolPredicate);
		}

		/// <summary>
		/// parse a string to see if it contains only digits or white spaces
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool ParseSimpleLogic(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return false;   // to avoid null or empty string passing through

			return VerifyValidChars(input, DigitsOrWhiteSpace_CharBoolPredicate);
		}

        /// <summary>
        /// parse a string to see if it contains only digits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ParseDigitLogic(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return false;   // to avoid null or empty string passing through

            return VerifyValidChars(input, Digits_CharBoolPredicate);
        }

        /// <summary>
        /// parse a string to see if it contains only digits or dash character '-' or whitespace or null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ParseComplexDigitLogic(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return true;   

            return VerifyValidChars(input, DigitsDashWhiteSpace_CharBoolPredicate);
        }

        /// <summary>
        /// parse a string to see if it contains only 10 digits or whitespace or null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ParsePhoneNumberLogic(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return true;   

            return VerifyValidChars(input, DigitsOrWhiteSpace_CharBoolPredicate) && (input.Length == 10);
        }
		
        
        /// <summary>
        /// parse a string to see if it contains only 5 digits or whitespace or null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ParseZipCodeLogic(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return true;   

            return VerifyValidChars(input, DigitsOrWhiteSpace_CharBoolPredicate) && (input.Length == 5);
        }

        /// <summary>
        /// parse a string to see if it contains digits, '$', or a decimal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ParseCurrency(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return true;  

            return VerifyValidChars(input, Currency_CharBoolPredicate);
        }
		
		#endregion

		
		#region Predicates

		/// <summary>
		/// Checks is the char given is a digit, white space, colon or comma
		/// </summary>
		/// <param name="c">Char to check</param>
		/// <returns>True if the char given is a digit, white space, colon or comma</returns>
		public static bool DigitsWhiteSpaceColonOrComma_CharBoolPredicate(char c)
		{
			return (char.IsDigit(c) || char.IsWhiteSpace(c) || c == ':' || c == ',');
		}

		/// <summary>
		/// Checks is the char given is a digit or white space.
		/// </summary>
		/// <param name="c">Char to check</param>
		/// <returns>True if the char given is a digit or a white space</returns>
		public static bool DigitsOrWhiteSpace_CharBoolPredicate(char c)
		{
			return (char.IsDigit(c) || char.IsWhiteSpace(c));
		}

        /// <summary>
        /// Checks is the char given is a digit.
        /// </summary>
        /// <param name="c">Char to check</param>
        /// <returns>True if the char given is a digit</returns>
        public static bool Digits_CharBoolPredicate(char c)
        {
            return (char.IsDigit(c));
        }

        /// <summary>
        /// Checks is the char given is a digit or a dash '-'.
        /// </summary>
        /// <param name="c">Char to check</param>
        /// <returns>True if the char given is a digit or a dash '-'</returns>
        public static bool DigitsDashWhiteSpace_CharBoolPredicate(char c)
        {
            return (char.IsDigit(c) || char.Equals(c,'-') || char.IsWhiteSpace(c));
        }

        /// <summary>
        /// Checks is the char given is a digit or a dash '-'.
        /// </summary>
        /// <param name="c">Char to check</param>
        /// <returns>True if the char given is a digit or a dash '-'</returns>
        public static bool Currency_CharBoolPredicate(char c)
        {
            return (char.IsDigit(c) || char.Equals(c,'$') || char.Equals(c,'.'));
        }
        
        
		#endregion
		
		/// <summary>
		/// This will make sure all the chars in the string satisfy the predicate condition
		/// </summary>
		/// <param name="input">The string we'll work with</param>
		/// <param name="pred">The predicate to check against</param>
		/// <returns>True if all chars satisfy the predicate</returns>
		public static bool VerifyValidChars(string input, Func<char,bool> pred)
		{
			return input.All(pred);
		}

	}

	
}