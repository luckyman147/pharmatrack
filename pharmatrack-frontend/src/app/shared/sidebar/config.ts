import { AppIcons } from '../../core/icons.enum';
import { SidebarRoute } from '../models/sidebar-route.model';

export const MAIN_ROUTES: SidebarRoute[] = [
  {
    path: '/',
    title: 'Tableau de Bord',
    icon: 'bi-house-door-fill',
    isExternalLink: false,
    submenu: [],
    badgeText: 7,
    badgeClass: 'bg-indigo-500',
  },
  {
    path: '/pharmacies',
    title: 'Pharmacies ',
    icon: AppIcons.PHARMACY  ,
    isExternalLink: false,
    isSvgIcon: true,
    
    submenu: [],
  },
  {
    path: '/delegues',
    title: 'Délégués',
    icon: 'bi-person-badge',
    isExternalLink: false,
    submenu: [],
  },
  {
    path: 'Transaction',
    title: 'Commandes',
    icon: 'bi-box-seam',
    isExternalLink: false,
    submenu: [
      {
        path: '/Transaction/visites',
        title: 'Visites',
        icon: 'bi-arrow-right-short',
        isExternalLink: false,
        submenu: [],
        badgeText: 'New',
      },
      {
        path: '/Transaction/Commandes',
        title: 'Sous-Commandes',
        icon: 'bi-arrow-right-short',
        isExternalLink: false,
        submenu: [],
      },
    ],
  },
  {
    path: '/depot',
    title: 'Gestion des grossistes',
    icon: 'bi-building',
    isExternalLink: false,
    submenu: [],
  },
  {
    path: '/rapport',
    title: 'Les Rapports',
    icon: 'bi-file-earmark-bar-graph',
    isExternalLink: false,
    submenu: [],
  },
];

export const PROFILE_ROUTES: SidebarRoute[] = [
  {
    path: '/settings',
    title: 'Settings',
    icon: 'bi-gear',
    isExternalLink: false,
    submenu: [],
  },
  {
    path: '/account',
    title: 'Account',
    icon: 'bi-person',
    isExternalLink: false,
    submenu: [],
  },
  {
    path: '/help',
    title: 'Help',
    icon: 'bi-question-circle',
    isExternalLink: false,
    submenu: [],
  },
  {
    path: '/logout',
    title: 'Log Out',
    icon: 'bi-box-arrow-right',
    isExternalLink: false,
    isLogout: true,
    submenu: [],
  },
];
