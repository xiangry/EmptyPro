.class final Lcom/unity3d/player/M;
.super Ljava/lang/Object;
.source "SourceFile"

# interfaces
.implements Landroid/content/DialogInterface$OnCancelListener;


# instance fields
.field final synthetic a:Lcom/unity3d/player/O;


# direct methods
.method constructor <init>(Lcom/unity3d/player/O;)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/M;->a:Lcom/unity3d/player/O;

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method


# virtual methods
.method public final onCancel(Landroid/content/DialogInterface;)V
    .locals 0

    iget-object p1, p0, Lcom/unity3d/player/M;->a:Lcom/unity3d/player/O;

    iget-object p1, p1, Lcom/unity3d/player/F;->f:Lcom/unity3d/player/G;

    if-eqz p1, :cond_0

    invoke-virtual {p1}, Lcom/unity3d/player/G;->a()V

    :cond_0
    return-void
.end method
