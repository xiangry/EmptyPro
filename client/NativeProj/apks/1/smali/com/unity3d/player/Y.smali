.class final Lcom/unity3d/player/Y;
.super Ljava/lang/Object;
.source "SourceFile"

# interfaces
.implements Ljava/lang/Runnable;


# instance fields
.field final synthetic a:Lcom/unity3d/player/UnityPlayer;

.field final synthetic b:Ljava/lang/String;

.field final synthetic c:I

.field final synthetic d:Z

.field final synthetic e:Z

.field final synthetic f:Z

.field final synthetic g:Z

.field final synthetic h:Ljava/lang/String;

.field final synthetic i:I

.field final synthetic j:Z

.field final synthetic k:Z

.field final synthetic l:Lcom/unity3d/player/UnityPlayer;


# direct methods
.method constructor <init>(Lcom/unity3d/player/UnityPlayer;Lcom/unity3d/player/UnityPlayer;Ljava/lang/String;IZZZZLjava/lang/String;IZZ)V
    .locals 0

    iput-object p1, p0, Lcom/unity3d/player/Y;->l:Lcom/unity3d/player/UnityPlayer;

    iput-object p2, p0, Lcom/unity3d/player/Y;->a:Lcom/unity3d/player/UnityPlayer;

    iput-object p3, p0, Lcom/unity3d/player/Y;->b:Ljava/lang/String;

    iput p4, p0, Lcom/unity3d/player/Y;->c:I

    iput-boolean p5, p0, Lcom/unity3d/player/Y;->d:Z

    iput-boolean p6, p0, Lcom/unity3d/player/Y;->e:Z

    iput-boolean p7, p0, Lcom/unity3d/player/Y;->f:Z

    iput-boolean p8, p0, Lcom/unity3d/player/Y;->g:Z

    iput-object p9, p0, Lcom/unity3d/player/Y;->h:Ljava/lang/String;

    iput p10, p0, Lcom/unity3d/player/Y;->i:I

    iput-boolean p11, p0, Lcom/unity3d/player/Y;->j:Z

    iput-boolean p12, p0, Lcom/unity3d/player/Y;->k:Z

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method


# virtual methods
.method public final run()V
    .locals 15

    iget-object v0, p0, Lcom/unity3d/player/Y;->l:Lcom/unity3d/player/UnityPlayer;

    invoke-static {}, Lcom/unity3d/player/SoftInputProvider;->a()I

    move-result v1

    iget-object v2, p0, Lcom/unity3d/player/Y;->l:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v2}, Lcom/unity3d/player/UnityPlayer;->-$$Nest$fgetmContext(Lcom/unity3d/player/UnityPlayer;)Landroid/content/Context;

    move-result-object v2

    iget-object v3, p0, Lcom/unity3d/player/Y;->a:Lcom/unity3d/player/UnityPlayer;

    iget-object v5, p0, Lcom/unity3d/player/Y;->b:Ljava/lang/String;

    iget v6, p0, Lcom/unity3d/player/Y;->c:I

    iget-boolean v7, p0, Lcom/unity3d/player/Y;->d:Z

    iget-boolean v8, p0, Lcom/unity3d/player/Y;->e:Z

    iget-boolean v9, p0, Lcom/unity3d/player/Y;->f:Z

    iget-boolean v10, p0, Lcom/unity3d/player/Y;->g:Z

    iget-object v11, p0, Lcom/unity3d/player/Y;->h:Ljava/lang/String;

    iget v12, p0, Lcom/unity3d/player/Y;->i:I

    iget-boolean v13, p0, Lcom/unity3d/player/Y;->j:Z

    iget-boolean v14, p0, Lcom/unity3d/player/Y;->k:Z

    invoke-static {v1}, Lcom/unity3d/player/a/a;->a(I)I

    move-result v1

    const/4 v4, 0x2

    if-eq v1, v4, :cond_0

    new-instance v1, Lcom/unity3d/player/O;

    invoke-direct {v1, v2, v3}, Lcom/unity3d/player/O;-><init>(Landroid/content/Context;Lcom/unity3d/player/UnityPlayer;)V

    goto :goto_0

    :cond_0
    new-instance v1, Lcom/unity3d/player/K;

    invoke-direct {v1, v2, v3}, Lcom/unity3d/player/K;-><init>(Landroid/content/Context;Lcom/unity3d/player/UnityPlayer;)V

    :goto_0
    move-object v4, v1

    invoke-virtual/range {v4 .. v14}, Lcom/unity3d/player/F;->a(Ljava/lang/String;IZZZZLjava/lang/String;IZZ)V

    iput-object v1, v0, Lcom/unity3d/player/UnityPlayer;->mSoftInput:Lcom/unity3d/player/F;

    iget-object v0, p0, Lcom/unity3d/player/Y;->l:Lcom/unity3d/player/UnityPlayer;

    iget-object v0, v0, Lcom/unity3d/player/UnityPlayer;->mSoftInput:Lcom/unity3d/player/F;

    new-instance v1, Lcom/unity3d/player/X;

    invoke-direct {v1, p0}, Lcom/unity3d/player/X;-><init>(Lcom/unity3d/player/Y;)V

    iput-object v1, v0, Lcom/unity3d/player/F;->f:Lcom/unity3d/player/G;

    invoke-virtual {v0}, Lcom/unity3d/player/F;->e()V

    iget-object v0, p0, Lcom/unity3d/player/Y;->l:Lcom/unity3d/player/UnityPlayer;

    invoke-static {v0}, Lcom/unity3d/player/UnityPlayer;->-$$Nest$mnativeReportKeyboardConfigChanged(Lcom/unity3d/player/UnityPlayer;)V

    return-void
.end method
