.class final Lcom/unity3d/player/j0;
.super Lcom/unity3d/player/D0;
.source "SourceFile"


# instance fields
.field final synthetic b:Z

.field final synthetic c:Lcom/unity3d/player/UnityPlayer;


# direct methods
.method constructor <init>(Lcom/unity3d/player/UnityPlayer;Z)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/j0;->c:Lcom/unity3d/player/UnityPlayer;

    iput-boolean p2, p0, Lcom/unity3d/player/j0;->b:Z

    const/4 p2, 0x0

    invoke-direct {p0, p1, p2}, Lcom/unity3d/player/D0;-><init>(Lcom/unity3d/player/UnityPlayer;Lcom/unity3d/player/D0-IA;)V

    return-void
.end method


# virtual methods
.method public final a()V
    .locals 2

    iget-object v0, p0, Lcom/unity3d/player/j0;->c:Lcom/unity3d/player/UnityPlayer;

    iget-boolean v1, p0, Lcom/unity3d/player/j0;->b:Z

    invoke-static {v0, v1}, Lcom/unity3d/player/UnityPlayer;->-$$Nest$mnativeSetKeyboardIsVisible(Lcom/unity3d/player/UnityPlayer;Z)V

    return-void
.end method
