namespace UwpClient.Helpers
{
    public static class Extensions
    {
        public static Models.Json.Record ToRecord(this Models.Record record)
        {
            return new Models.Json.Record
            {
                id = record.Id
            };
        }

        public static Models.Record ToRecord(this Models.Json.Record record)
        {
            return new Models.Record
            {
                Id = record.id,
            };
        }
    }
}
