﻿namespace Clean.Architecture.Core.ContributorAggregate;

public class Contributor(ContributorName name) : EntityBase<Contributor, ContributorId>, IAggregateRoot
{
  public ContributorName Name { get; private set; } = name;
  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;
  public PhoneNumber? PhoneNumber { get; private set; }

  public Contributor UpdatePhoneNumber(PhoneNumber newPhoneNumber)
  {
    PhoneNumber = newPhoneNumber;
    return this;
  }

  public Contributor UpdateName(ContributorName newName)
  {
    Name = newName;
    return this;
  }
}
