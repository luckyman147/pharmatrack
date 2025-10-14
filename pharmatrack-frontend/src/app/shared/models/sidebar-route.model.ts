import { signal, WritableSignal } from '@angular/core';

// Simport { WritableSignal, signal } from '@angular/core';

// Simport { WritableSignal, signal } from '@angular/core';
export class SidebarRoute {
  path!: string;
  title!: string;
  icon!: string;
  isExternalLink?: boolean;
  submenu!: SidebarRoute[];
  // expanded is WritableSignal<boolean> only if submenu is present
  expanded?: WritableSignal<boolean>;
  isSvgIcon?: boolean;
  isLogout?: boolean;
  badgeText?: string | number | null;
  badgeClass?: string;
}
