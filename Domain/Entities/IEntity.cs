﻿using System;

namespace Domain.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
