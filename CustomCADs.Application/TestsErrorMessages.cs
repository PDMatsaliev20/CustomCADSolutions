namespace CustomCADs.Application
{
    public static class TestsErrorMessages
    {
        public const string ModelsCountMismatch = "{0} Count mismatch.";

        public const string ExistsButCannotFind = "Couldn't find existing {0}.";
        public const string FindsButDoesNotExist = "Finds non-existent {0}.";
        public const string ModelPropertyMismatch = "{0} mismatch.";

        public const string AddedNull = "Added null {0}.";

        public const string DoesNotEditEnough = "New {0} doesn't get saved.";
        public const string EditsTooMuch = "New {0} gets saved.";
        public const string EditsNonExistent = "Non-existent {0} edited.";

        public const string DidNotDelete = "Found deleted {0}.";
        public const string ShouldNotHaveDeleted = "Deleted non-existent {0}.";

        public const string DidNotFind = "Didn't find existing {0}.";
        public const string ShouldNotHaveFound = "Found non-existent {0}.";

        public const string OrderNotFinished = "Order was not tagged as Finished.";
        public const string ShoultHaveBeenSaved = "Cad's {0} doesn't get saved.";

        public const string CountedWrong = "Incorrect count.";
        
        public const string IncorrectCountByFilter = "Incorrect count by filter/s {0}";
        public const string IncorrectSortByFilter = "Incorrect sort by filter/s {0}";
        public const string IncorrectPaging = "Incorrect count of cads per page on page {0}";
    }
}
