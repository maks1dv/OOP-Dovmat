using System;

namespace lab14v8
{
    public class Transaction
    {
        public string Id { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"[ID: {Id}] {Sender} -> {Receiver}: {Amount:C} ({Timestamp:HH:mm:ss})";
        }
    }
}
