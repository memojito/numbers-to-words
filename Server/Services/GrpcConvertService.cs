using System.Text;
using Grpc.Core;

namespace Server.Services
{
    /// <summary>
    /// Service for converting numbers to words.
    /// </summary>
    public class GrpcConvertService(ILogger<GrpcConvertService> logger) : GrpcConvert.GrpcConvertBase
    {
        private readonly ILogger<GrpcConvertService> _logger = logger;

        /// <summary>
        /// Converts a numeric amount from a gRPC request to its word representation.
        /// </summary>
        /// <param name="request">The request containing the numeric input.</param>
        /// <param name="context">The server call context.</param>
        /// <returns>A task with the converted amount in words.</returns>
        public override Task<ConvertResponse> Convert(ConvertRequest request, ServerCallContext context)
        {
            double amount = request.Input;

            // validate input
            if (!IsPositiveNumber(amount))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Please enter a positive number."));
            }

            if (!HasUpToTwoDecimalPlaces(amount))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Please enter a number with no more than 2 decimal places."));
            }

            _logger.LogInformation("Converting {amount} to a word:", amount);

            // handle zero
            if (amount == 0)
            {
                return Task.FromResult(new ConvertResponse { Output = "zero dollars" });
            }

            // separate dollars and cents and convert
            long dollars = (long)Math.Floor(amount);
            int cents = (int)Math.Round((amount - dollars) * 100);

            string dollarsText = ConvertToWords(dollars);
            string centsText = ConvertToWords(cents);

            // add the words dollars and cents
            string result;
            if (dollars == 0)
            {
                result = $"{centsText} {(cents == 1 ? "cent" : "cents")}";
            }
            else if (cents == 0)
            {
                result = $"{dollarsText} {(dollars == 1 ? "dollar" : "dollars")}";
            }
            else
            {
                result = $"{dollarsText} {(dollars == 1 ? "dollar" : "dollars")} and {centsText} {(cents == 1 ? "cent" : "cents")}";
            }

            return Task.FromResult(new ConvertResponse { Output = result });
        }

        /// <summary>
        ///  Method <c>ConvertToWords</c> converts a numeric value to its textual representation in English words.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>The textual representation of the number in words.</returns>
        private static string ConvertToWords(long number)
        {
            if (number == 0)
            {
                return "zero";
            }

            string[] ones = ["", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
            string[] teens = ["ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];
            string[] tens = ["", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

            StringBuilder result = new();

            if (number >= 1000000000)
            {
                result.Append(ConvertToWords(number / 1000000000) + " billion ");
                number %= 1000000000;
            }

            if (number >= 1000000)
            {
                result.Append(ConvertToWords(number / 1000000) + " million ");
                number %= 1000000;
            }

            if (number >= 1000)
            {
                result.Append(ConvertToWords(number / 1000) + " thousand ");
                number %= 1000;
            }

            if (number >= 100)
            {
                result.Append(ones[number / 100] + " hundred ");
                number %= 100;
            }

            if (number >= 20)
            {
                result.Append(tens[number / 10] + " ");
                number %= 10;
            }
            else if (number >= 10)
            {
                result.Append(teens[number - 10] + " ");
                number = 0;
            }

            if (number > 0 && number < 10)
            {
                // add the hyphen
                if (result.Length > 0 && result[^1] != '-')
                {
                    // trim whitespace before appending
                    result.Remove(result.Length - 1, 1);
                    result.Append('-');
                }
                result.Append(ones[number]);
            }

            return result.ToString().Trim();
        }
        private static bool IsPositiveNumber(double number)
        {
            return number >= 0;
        }

        public bool HasUpToTwoDecimalPlaces(double number)
        {
            double scaled = number * 100;
            return Math.Abs(scaled - Math.Round(scaled)) < 0.0001;
        }
    }
}
