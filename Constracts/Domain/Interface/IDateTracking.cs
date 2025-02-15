﻿namespace Constracts.Domain.Interface
{
    public interface IDateTracking
    {
        DateTimeOffset CreatedDate { get; set; }

        DateTimeOffset? LastModifiedDate { get; set; }
    }
}
