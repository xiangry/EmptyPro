.class final Lcom/unity3d/player/n0;
.super Landroid/view/OrientationEventListener;
.source "SourceFile"


# instance fields
.field final synthetic a:Lcom/unity3d/player/UnityPlayer;


# direct methods
.method constructor <init>(Lcom/unity3d/player/UnityPlayer;Landroid/content/Context;I)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/n0;->a:Lcom/unity3d/player/UnityPlayer;

    invoke-direct {p0, p2, p3}, Landroid/view/OrientationEventListener;-><init>(Landroid/content/Context;I)V

    return-void
.end method


# virtual methods
.method public final onOrientationChanged(I)V
    .locals 2

    iget-object v0, p0, Lcom/unity3d/player/n0;->a:Lcom/unity3d/player/UnityPlayer;

    iget-object v1, v0, Lcom/unity3d/player/UnityPlayer;->m_MainThread:Lcom/unity3d/player/C0;

    invoke-static {v0}, Lcom/unity3d/player/UnityPlayer;->-$$Nest$fgetmNaturalOrientation(Lcom/unity3d/player/UnityPlayer;)I

    move-result v0

    iput v0, v1, Lcom/unity3d/player/C0;->f:I

    iput p1, v1, Lcom/unity3d/player/C0;->g:I

    sget-object p1, Lcom/unity3d/player/A0;->j:Lcom/unity3d/player/A0;

    invoke-static {v1, p1}, Lcom/unity3d/player/C0;->-$$Nest$ma(Lcom/unity3d/player/C0;Lcom/unity3d/player/A0;)V

    return-void
.end method
