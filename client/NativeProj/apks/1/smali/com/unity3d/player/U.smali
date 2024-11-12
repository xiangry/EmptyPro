.class final Lcom/unity3d/player/U;
.super Ljava/lang/Object;
.source "SourceFile"

# interfaces
.implements Landroid/view/SurfaceHolder$Callback;


# instance fields
.field final synthetic a:Lcom/unity3d/player/V;


# direct methods
.method constructor <init>(Lcom/unity3d/player/V;)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/U;->a:Lcom/unity3d/player/V;

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method


# virtual methods
.method public final surfaceChanged(Landroid/view/SurfaceHolder;III)V
    .locals 0

    iget-object p2, p0, Lcom/unity3d/player/U;->a:Lcom/unity3d/player/V;

    invoke-static {p2}, Lcom/unity3d/player/V;->-$$Nest$fgetb(Lcom/unity3d/player/V;)Lcom/unity3d/player/UnityPlayer;

    move-result-object p2

    invoke-interface {p1}, Landroid/view/SurfaceHolder;->getSurface()Landroid/view/Surface;

    move-result-object p1

    const/4 p3, 0x0

    invoke-virtual {p2, p3, p1}, Lcom/unity3d/player/UnityPlayer;->updateGLDisplay(ILandroid/view/Surface;)V

    iget-object p1, p0, Lcom/unity3d/player/U;->a:Lcom/unity3d/player/V;

    invoke-static {p1}, Lcom/unity3d/player/V;->-$$Nest$fgetb(Lcom/unity3d/player/V;)Lcom/unity3d/player/UnityPlayer;

    move-result-object p1

    invoke-virtual {p1}, Lcom/unity3d/player/UnityPlayer;->sendSurfaceChangedEvent()V

    return-void
.end method

.method public final surfaceCreated(Landroid/view/SurfaceHolder;)V
    .locals 2

    iget-object v0, p0, Lcom/unity3d/player/U;->a:Lcom/unity3d/player/V;

    invoke-static {v0}, Lcom/unity3d/player/V;->-$$Nest$fgetb(Lcom/unity3d/player/V;)Lcom/unity3d/player/UnityPlayer;

    move-result-object v0

    invoke-interface {p1}, Landroid/view/SurfaceHolder;->getSurface()Landroid/view/Surface;

    move-result-object p1

    const/4 v1, 0x0

    invoke-virtual {v0, v1, p1}, Lcom/unity3d/player/UnityPlayer;->updateGLDisplay(ILandroid/view/Surface;)V

    iget-object p1, p0, Lcom/unity3d/player/U;->a:Lcom/unity3d/player/V;

    invoke-static {p1}, Lcom/unity3d/player/V;->-$$Nest$fgetc(Lcom/unity3d/player/V;)Lcom/unity3d/player/y;

    move-result-object v0

    invoke-static {p1}, Lcom/unity3d/player/V;->-$$Nest$fgetb(Lcom/unity3d/player/V;)Lcom/unity3d/player/UnityPlayer;

    move-result-object p1

    iget-object v1, v0, Lcom/unity3d/player/y;->b:Lcom/unity3d/player/x;

    if-eqz v1, :cond_0

    invoke-virtual {v1}, Landroid/view/View;->getParent()Landroid/view/ViewParent;

    move-result-object v1

    if-nez v1, :cond_0

    iget-object v1, v0, Lcom/unity3d/player/y;->b:Lcom/unity3d/player/x;

    invoke-virtual {p1, v1}, Landroid/view/ViewGroup;->addView(Landroid/view/View;)V

    iget-object v0, v0, Lcom/unity3d/player/y;->b:Lcom/unity3d/player/x;

    invoke-virtual {p1, v0}, Landroid/view/ViewGroup;->bringChildToFront(Landroid/view/View;)V

    :cond_0
    return-void
.end method

.method public final surfaceDestroyed(Landroid/view/SurfaceHolder;)V
    .locals 3

    iget-object p1, p0, Lcom/unity3d/player/U;->a:Lcom/unity3d/player/V;

    invoke-static {p1}, Lcom/unity3d/player/V;->-$$Nest$fgetc(Lcom/unity3d/player/V;)Lcom/unity3d/player/y;

    move-result-object v0

    invoke-static {p1}, Lcom/unity3d/player/V;->-$$Nest$fgeta(Lcom/unity3d/player/V;)Lcom/unity3d/player/a;

    move-result-object p1

    invoke-virtual {v0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    sget-boolean v1, Lcom/unity3d/player/PlatformSupport;->NOUGAT_SUPPORT:Z

    if-eqz v1, :cond_1

    iget-object v1, v0, Lcom/unity3d/player/y;->a:Landroid/content/Context;

    if-eqz v1, :cond_1

    iget-object v1, v0, Lcom/unity3d/player/y;->b:Lcom/unity3d/player/x;

    if-nez v1, :cond_0

    new-instance v1, Lcom/unity3d/player/x;

    iget-object v2, v0, Lcom/unity3d/player/y;->a:Landroid/content/Context;

    invoke-direct {v1, v2}, Lcom/unity3d/player/x;-><init>(Landroid/content/Context;)V

    iput-object v1, v0, Lcom/unity3d/player/y;->b:Lcom/unity3d/player/x;

    :cond_0
    iget-object v0, v0, Lcom/unity3d/player/y;->b:Lcom/unity3d/player/x;

    invoke-virtual {v0, p1}, Lcom/unity3d/player/x;->a(Landroid/view/SurfaceView;)V

    :cond_1
    iget-object p1, p0, Lcom/unity3d/player/U;->a:Lcom/unity3d/player/V;

    invoke-static {p1}, Lcom/unity3d/player/V;->-$$Nest$fgetb(Lcom/unity3d/player/V;)Lcom/unity3d/player/UnityPlayer;

    move-result-object p1

    const/4 v0, 0x0

    const/4 v1, 0x0

    invoke-virtual {p1, v0, v1}, Lcom/unity3d/player/UnityPlayer;->updateGLDisplay(ILandroid/view/Surface;)V

    return-void
.end method
