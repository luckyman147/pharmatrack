import { DelegueInfo } from "src/app/transaction/visite/models/DelegueInfo";
import { User } from "../Models/User";

export function toDelegueInfo(user: User): DelegueInfo {
  if (!user) {
    throw new Error('User cannot be null or undefined');
  }

  return {
    id: user.id,
    fullName: user.fullName,
    avatarUrl: user.profile?.avatarUrl || null,
  };
}
