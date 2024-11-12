.class final Lcom/unity3d/player/L;
.super Ljava/lang/Object;
.source "SourceFile"

# interfaces
.implements Landroid/view/ViewTreeObserver$OnGlobalLayoutListener;


# instance fields
.field final synthetic a:Lcom/unity3d/player/O;


# direct methods
.method constructor <init>(Lcom/unity3d/player/O;)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/L;->a:Lcom/unity3d/player/O;

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method


# virtual methods
.method public final onGlobalLayout()V
    .locals 1

    iget-object v0, p0, Lcom/unity3d/player/L;->a:Lcom/unity3d/player/O;

    invoke-virtual {v0}, Lcom/unity3d/player/O;->reportSoftInputArea()V

    return-void
.end method
