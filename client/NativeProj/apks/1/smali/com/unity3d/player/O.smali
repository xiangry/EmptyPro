.class final Lcom/unity3d/player/O;
.super Lcom/unity3d/player/F;
.source "SourceFile"


# instance fields
.field g:Lcom/unity3d/player/H;


# direct methods
.method public constructor <init>(Landroid/content/Context;Lcom/unity3d/player/UnityPlayer;)V
    .locals 0

    invoke-direct {p0, p1, p2}, Lcom/unity3d/player/F;-><init>(Landroid/content/Context;Lcom/unity3d/player/UnityPlayer;)V

    return-void
.end method


# virtual methods
.method public final a(Ljava/lang/String;IZZZZLjava/lang/String;IZZ)V
    .locals 3

    new-instance v0, Lcom/unity3d/player/H;

    iget-object v1, p0, Lcom/unity3d/player/F;->a:Landroid/content/Context;

    iget-object v2, p0, Lcom/unity3d/player/F;->b:Lcom/unity3d/player/UnityPlayer;

    invoke-direct {v0, v1, v2}, Lcom/unity3d/player/H;-><init>(Landroid/content/Context;Lcom/unity3d/player/UnityPlayer;)V

    iput-object v0, p0, Lcom/unity3d/player/O;->g:Lcom/unity3d/player/H;

    invoke-virtual {v0, p0, p9, p10}, Lcom/unity3d/player/H;->a(Lcom/unity3d/player/F;ZZ)V

    invoke-super/range {p0 .. p10}, Lcom/unity3d/player/F;->a(Ljava/lang/String;IZZZZLjava/lang/String;IZZ)V

    iget-object p1, p0, Lcom/unity3d/player/F;->b:Lcom/unity3d/player/UnityPlayer;

    invoke-virtual {p1}, Landroid/view/View;->getViewTreeObserver()Landroid/view/ViewTreeObserver;

    move-result-object p1

    new-instance p2, Lcom/unity3d/player/L;

    invoke-direct {p2, p0}, Lcom/unity3d/player/L;-><init>(Lcom/unity3d/player/O;)V

    invoke-virtual {p1, p2}, Landroid/view/ViewTreeObserver;->addOnGlobalLayoutListener(Landroid/view/ViewTreeObserver$OnGlobalLayoutListener;)V

    iget-object p1, p0, Lcom/unity3d/player/F;->c:Landroid/widget/EditText;

    invoke-virtual {p1}, Landroid/view/View;->requestFocus()Z

    iget-object p1, p0, Lcom/unity3d/player/O;->g:Lcom/unity3d/player/H;

    new-instance p2, Lcom/unity3d/player/M;

    invoke-direct {p2, p0}, Lcom/unity3d/player/M;-><init>(Lcom/unity3d/player/O;)V

    invoke-virtual {p1, p2}, Landroid/app/Dialog;->setOnCancelListener(Landroid/content/DialogInterface$OnCancelListener;)V

    return-void
.end method

.method public final a(Z)V
    .locals 1

    iput-boolean p1, p0, Lcom/unity3d/player/F;->d:Z

    iget-object v0, p0, Lcom/unity3d/player/O;->g:Lcom/unity3d/player/H;

    invoke-virtual {v0, p1}, Lcom/unity3d/player/H;->a(Z)V

    return-void
.end method

.method public final c()V
    .locals 1

    iget-object v0, p0, Lcom/unity3d/player/O;->g:Lcom/unity3d/player/H;

    invoke-virtual {v0}, Landroid/app/Dialog;->dismiss()V

    return-void
.end method

.method protected createEditText(Lcom/unity3d/player/F;)Landroid/widget/EditText;
    .locals 2

    new-instance v0, Lcom/unity3d/player/N;

    iget-object v1, p0, Lcom/unity3d/player/F;->a:Landroid/content/Context;

    invoke-direct {v0, v1, p1}, Lcom/unity3d/player/N;-><init>(Landroid/content/Context;Lcom/unity3d/player/F;)V

    return-object v0
.end method

.method public final e()V
    .locals 1

    iget-object v0, p0, Lcom/unity3d/player/O;->g:Lcom/unity3d/player/H;

    invoke-virtual {v0}, Landroid/app/Dialog;->show()V

    return-void
.end method

.method protected reportSoftInputArea()V
    .locals 2

    iget-object v0, p0, Lcom/unity3d/player/O;->g:Lcom/unity3d/player/H;

    invoke-virtual {v0}, Landroid/app/Dialog;->isShowing()Z

    move-result v0

    if-eqz v0, :cond_0

    iget-object v0, p0, Lcom/unity3d/player/O;->g:Lcom/unity3d/player/H;

    invoke-virtual {v0}, Lcom/unity3d/player/H;->a()Landroid/graphics/Rect;

    move-result-object v0

    iget-object v1, p0, Lcom/unity3d/player/F;->b:Lcom/unity3d/player/UnityPlayer;

    invoke-virtual {v1, v0}, Lcom/unity3d/player/UnityPlayer;->reportSoftInputArea(Landroid/graphics/Rect;)V

    :cond_0
    return-void
.end method
