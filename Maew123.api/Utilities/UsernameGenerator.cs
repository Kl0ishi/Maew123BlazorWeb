namespace Maew123.Api.Utilities
{
    public static class UsernameGenerator
    {
        private static HashSet<string> UsedUsernames = new HashSet<string>();

        public static string GenerateUsername(string firstName, string lastName, string email)
        {
            string baseUsername = $"{firstName.ToLower()}.{lastName.ToLower()}";

            // Extract characters before the '@' symbol from the email
            string emailPrefix = email.Split('@')[0].ToLower();

            // Combine the base username and email prefix
            string combinedUsername = $"{baseUsername}.{emailPrefix}";

            // Ensure the combined username is unique
            string uniqueUsername = combinedUsername;
            int counter = 1;
            while (UsedUsernames.Contains(uniqueUsername))
            {
                uniqueUsername = $"{combinedUsername}{counter}";
                counter++;
            }

            UsedUsernames.Add(uniqueUsername);

            return uniqueUsername;
        }
    }
}
