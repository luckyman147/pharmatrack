import { Routes } from '@angular/router';

//Route for content layout with sidebar, navbar and footer.

export const Full_ROUTES: Routes = [
    {
            data: { 
      breadcrumbText: 'Tableau de Bord' // <--- The text you want to control
    } ,
        path: 'dashboard',
        loadChildren: () => import('../../dashboard/dashboard.module').then(m => m.DashboardModule)
    },
    {
        path: 'Transaction',
        loadChildren: () => import('../../transaction/transaction.module').then(m => m.TransactionModule)

    },
    {
            data: { 
      breadcrumbText: 'Widget' // <--- The text you want to control
    } ,
        path: 'widgets',
        loadChildren: () => import('../../widgets/widgets.module').then(m => m.WidgetsModule)

    },
    {
        path: 'produits',
        loadChildren: () => import('../../produits/produits.module').then(m => m.ProduitsModule)

    },
{
    path: 'depot',
    loadChildren: () => import('../../depots/depots.module').then(m => m.DepotsModule)
},
{
    path: 'commandes',
    loadChildren: () => import('../../commandes/commandes.module').then(m => m.CommandesModule)
},

    {
        path: 'categorie',
        loadChildren: () => import('../../produits/categorie/categorie.module').then(m => m.CategorieModule)
    
      },
      {
        path: 'histoires',
        loadChildren: () => import('../../histoires/histoires.module').then(m => m.HistoiresModule)
      },
     
  
    // {
    //     path: 'maps',
    //     loadChildren: () => import('../../maps/maps.module').then(m => m.MapsModule)

    // },
    // {
    //     path: 'error',
    //     loadChildren: () => import('./../../error/error.module').then(m => m.ErrorModule)
    // },
];