.class final Lcom/unity3d/player/I0;
.super Ljava/lang/Object;
.source "SourceFile"

# interfaces
.implements Ljava/lang/Runnable;


# instance fields
.field final synthetic a:Lcom/unity3d/player/J0;


# direct methods
.method constructor <init>(Lcom/unity3d/player/J0;)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/I0;->a:Lcom/unity3d/player/J0;

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method


# virtual methods
.method public final run()V
    .locals 3

    iget-object v0, p0, Lcom/unity3d/player/I0;->a:Lcom/unity3d/player/J0;

    iget-object v0, v0, Lcom/unity3d/player/J0;->a:Lcom/unity3d/player/K0;

    iget-object v0, v0, Lcom/unity3d/player/K0;->h:Lcom/unity3d/player/P0;

    invoke-static {v0}, Lcom/unity3d/player/P0;->-$$Nest$fgetf(Lcom/unity3d/player/P0;)Lcom/unity3d/player/H0;

    move-result-object v1

    if-eqz v1, :cond_0

    invoke-static {v0}, Lcom/unity3d/player/P0;->-$$Nest$fgeta(Lcom/unity3d/player/P0;)Lcom/unity3d/player/UnityPlayer;

    move-result-object v2

    invoke-virtual {v2, v1}, Lcom/unity3d/player/UnityPlayer;->removeViewFromPlayer(Landroid/view/View;)V

    const/4 v1, 0x0

    invoke-static {v0, v1}, Lcom/unity3d/player/P0;->-$$Nest$fputi(Lcom/unity3d/player/P0;Z)V

    invoke-static {v0}, Lcom/unity3d/player/P0;->-$$Nest$fgetf(Lcom/unity3d/player/P0;)Lcom/unity3d/player/H0;

    move-result-object v1

    invoke-virtual {v1}, Lcom/unity3d/player/H0;->destroyPlayer()V

    const/4 v1, 0x0

    invoke-static {v0, v1}, Lcom/unity3d/player/P0;->-$$Nest$fputf(Lcom/unity3d/player/P0;Lcom/unity3d/player/H0;)V

    invoke-static {v0}, Lcom/unity3d/player/P0;->-$$Nest$fgetc(Lcom/unity3d/player/P0;)Lcom/unity3d/player/O0;

    move-result-object v0

    if-eqz v0, :cond_0

    check-cast v0, Lcom/unity3d/player/k0;

    invoke-virtual {v0}, Lcom/unity3d/player/k0;->a()V

    :cond_0
    iget-object v0, p0, Lcom/unity3d/player/I0;->a:Lcom/unity3d/player/J0;

    iget-object v0, v0, Lcom/unity3d/player/J0;->a:Lcom/unity3d/player/K0;

    iget-object v0, v0, Lcom/unity3d/player/K0;->h:Lcom/unity3d/player/P0;

    invoke-static {v0}, Lcom/unity3d/player/P0;->-$$Nest$fgeta(Lcom/unity3d/player/P0;)Lcom/unity3d/player/UnityPlayer;

    move-result-object v0

    invoke-virtual {v0}, Lcom/unity3d/player/UnityPlayer;->onResume()V

    return-void
.end method
