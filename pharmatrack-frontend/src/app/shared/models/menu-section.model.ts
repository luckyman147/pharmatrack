import { SidebarRoute } from "./sidebar-route.model";

export interface MenuSection {
  header: string | null;
  separator: boolean; // Add a separator line after the section
  items: SidebarRoute[];
}
