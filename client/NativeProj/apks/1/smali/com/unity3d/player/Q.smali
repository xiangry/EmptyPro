.class final Lcom/unity3d/player/Q;
.super Landroid/database/ContentObserver;
.source "SourceFile"


# instance fields
.field private a:Lcom/unity3d/player/P;


# direct methods
.method public constructor <init>(Landroid/os/Handler;Lcom/unity3d/player/P;)V
    .locals 0

    invoke-direct {p0, p1}, Landroid/database/ContentObserver;-><init>(Landroid/os/Handler;)V

    iput-object p2, p0, Lcom/unity3d/player/Q;->a:Lcom/unity3d/player/P;

    return-void
.end method


# virtual methods
.method public final deliverSelfNotifications()Z
    .locals 1

    invoke-super {p0}, Landroid/database/ContentObserver;->deliverSelfNotifications()Z

    move-result v0

    return v0
.end method

.method public final onChange(Z)V
    .locals 0

    iget-object p1, p0, Lcom/unity3d/player/Q;->a:Lcom/unity3d/player/P;

    if-eqz p1, :cond_0

    check-cast p1, Lcom/unity3d/player/OrientationLockListener;

    invoke-virtual {p1}, Lcom/unity3d/player/OrientationLockListener;->b()V

    :cond_0
    return-void
.end method
