import { Routes } from '@angular/router';
import { AuthGuard } from './features/auth/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'Home', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'Home',
    loadComponent: () => import('./layout/full-layout/full-layout.component').then((m) => m.FullLayoutComponent),
    //TODO: Uncomment the line below to enable the AuthGuard
    // canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./features/dashboard/dashboard-routing.module').then(
            (m) => m.DashboardRoutingModule
          ),
      },
    ],
  },
];
