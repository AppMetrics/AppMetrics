// Counter Basic

var sentEmailsCounter = new CounterOptions
{
    Name = "Sent Emails",
    MeasurementUnit = Unit.Calls
} 

_metrics.Increment(sentEmailsCounter); // Increment by 1 
_metrics.Decrement(sentEmailsCounter); // Decrement by 1
_metrics.Increment(sentEmailsCounter, 10); // Increment by 10
_metrics.Decrement(sentEmailsCounter, 10); // Decrement by 10

// Counter Items

var sentEmailsCounter = new CounterOptions
{
    Name = "Sent Emails",
    MeasurementUnit = Unit.Calls
} 

_metrics.Increment(sentEmailsCounter, 70, "email-a-friend");
_metrics.Increment(sentEmailsCounter, 10, "forgot-password");
_metrics.Increment(sentEmailsCounter, 20, "account-verification");

// Counter Advanced

_metrics.Advanced.Counter(sentEmailsCounter).Reset();