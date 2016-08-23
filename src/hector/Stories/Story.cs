namespace Hector.Stories {
    using System;

    public class Story {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Closed { get; set; }
    }
}
