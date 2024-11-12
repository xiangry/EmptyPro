.class final Lcom/unity3d/player/X;
.super Lcom/unity3d/player/G;
.source "SourceFile"


# instance fields
.field final synthetic a:Lcom/unity3d/player/Y;


# direct methods
.method constructor <init>(Lcom/unity3d/player/Y;)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/X;->a:Lcom/unity3d/player/Y;

    invoke-direct {p0}, Lcom/unity3d/player/G;-><init>()V

    return-void
.end method


# virtual methods
.method public final a()V
    .locals 4

    iget-object v0, p0, Lcom/unity3d/player/X;->a:Lcom/unity3d/player/Y;

    iget-object v0, v0, Lcom/unity3d/player/Y;->l:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v0}, Lcom/unity3d/player/UnityPlayer;->-$$Nest$mnativeSoftInputLostFocus(Lcom/unity3d/player/UnityPlayer;)V

    iget-object v0, p0, Lcom/unity3d/player/X;->a:Lcom/unity3d/player/Y;

    iget-object v0, v0, Lcom/unity3d/player/Y;->l:Lcom/unity3d/player/UnityPlayer;

    const/4 v1, 0x1

    const/4 v2, 0x0

    const/4 v3, 0x0

    invoke-virtual {v0, v3, v1, v2}, Lcom/unity3d/player/UnityPlayer;->reportSoftInputStr(Ljava/lang/String;IZ)V

    return-void
.end method
