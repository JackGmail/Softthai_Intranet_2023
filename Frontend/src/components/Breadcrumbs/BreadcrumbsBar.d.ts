export interface IBreadcrumb {
    Item?: IBreadcrumbItem[];
}

export interface IBreadcrumbItem {
    nMenuID: number;
    sMenuName?: string;
    sRoute?: string;
    nLevel?: number;
    sIcon?: string;
}

