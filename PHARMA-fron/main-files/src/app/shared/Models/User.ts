import { UserProfile } from "./UserProfile";

export abstract class User {
    createdAt:Date;
    updatedAt:Date;
  id!: string; // Guid equivalent
  code!: string; // login code / unique identifier
  fullName!: string;
  email!: string;
  passwordHash!: string;
  emailConfirmed: boolean = false;
  profile?: UserProfile;

  constructor(init?: Partial<User>) {
    Object.assign(this, init);
  }

  get role(): string {
    // dynamically get class name like C#'s GetType().Name
    return this.constructor.name;
  }

  confirmEmail(): void {
    this.emailConfirmed = true;
  }

  updatePassword(newHash: string): void {
    this.passwordHash = newHash;
  }

  setProfile(profile: UserProfile): void {
    this.profile = profile;
  }
}
export class Admin extends User {
  constructor(init?: Partial<Admin>) {
    super(init);
  }
}
export class Delegue extends User {


  salary!: number;
  isActive: boolean = true;

  constructor(init?: Partial<Delegue>) {
    super(init);
    Object.assign(this, init);
  }

  activate(): void {
    this.isActive = true;
  }

  deactivate(): void {
    this.isActive = false;
  }

  updateSalary(newSalary: number): void {
    this.salary = newSalary;
  }
}