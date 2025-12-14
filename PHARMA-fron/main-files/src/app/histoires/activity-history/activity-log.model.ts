export interface ActivityLog {
    id: string;
    userId: string;
    userName: string;
    userRole: string;
    action: string;
    entityType: string;
    entityId?: string;
    description: string;
    timestamp: Date;
}

export interface ActivityHistoryResponse {
    result: {
        pageIndex: number;
        pageSize: number;
        count: number;
        data: ActivityLog[];
        pagesReste?: number;
    }
}
