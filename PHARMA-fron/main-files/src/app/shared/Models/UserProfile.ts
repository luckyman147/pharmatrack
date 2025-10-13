export class Address {
  street?: string;
  city?: string;
  postalCode?: string;
  country?: string;

  constructor(init?: Partial<Address>) {
    Object.assign(this, init);
  }
}

export class UserProfile {
  phoneNumber?: string;
  speciality?: string; // useful for delegues, doctors, etc.
  avatarUrl?: string;
  address?: Address; // profile picture

  constructor(init?: Partial<UserProfile>) {
    Object.assign(this, init);
  }
}
