.class public Lcom/unity3d/player/PlatformSupport;
.super Ljava/lang/Object;
.source "SourceFile"


# static fields
.field static final MARSHMALLOW_SUPPORT:Z

.field static final NOUGAT_SUPPORT:Z

.field static final PIE_SUPPORT:Z

.field static final RED_VELVET_CAKE_SUPPORT:Z

.field static final SNOW_CONE_SUPPORT:Z

.field static final TIRAMISU_SUPPORT:Z

.field static final UPSIDE_DOWN_CAKE_SUPPORT:Z

.field static final VANILLA_ICE_CREAM_SUPPORT:Z


# direct methods
.method static constructor <clinit>()V
    .locals 4

    sget v0, Landroid/os/Build$VERSION;->SDK_INT:I

    const/16 v1, 0x17

    const/4 v2, 0x1

    const/4 v3, 0x0

    if-lt v0, v1, :cond_0

    const/4 v1, 0x1

    goto :goto_0

    :cond_0
    const/4 v1, 0x0

    :goto_0
    sput-boolean v1, Lcom/unity3d/player/PlatformSupport;->MARSHMALLOW_SUPPORT:Z

    const/16 v1, 0x18

    if-lt v0, v1, :cond_1

    const/4 v1, 0x1

    goto :goto_1

    :cond_1
    const/4 v1, 0x0

    :goto_1
    sput-boolean v1, Lcom/unity3d/player/PlatformSupport;->NOUGAT_SUPPORT:Z

    const/16 v1, 0x1c

    if-lt v0, v1, :cond_2

    const/4 v1, 0x1

    goto :goto_2

    :cond_2
    const/4 v1, 0x0

    :goto_2
    sput-boolean v1, Lcom/unity3d/player/PlatformSupport;->PIE_SUPPORT:Z

    const/16 v1, 0x1e

    if-lt v0, v1, :cond_3

    const/4 v1, 0x1

    goto :goto_3

    :cond_3
    const/4 v1, 0x0

    :goto_3
    sput-boolean v1, Lcom/unity3d/player/PlatformSupport;->RED_VELVET_CAKE_SUPPORT:Z

    const/16 v1, 0x1f

    if-lt v0, v1, :cond_4

    const/4 v1, 0x1

    goto :goto_4

    :cond_4
    const/4 v1, 0x0

    :goto_4
    sput-boolean v1, Lcom/unity3d/player/PlatformSupport;->SNOW_CONE_SUPPORT:Z

    const/16 v1, 0x21

    if-lt v0, v1, :cond_5

    const/4 v1, 0x1

    goto :goto_5

    :cond_5
    const/4 v1, 0x0

    :goto_5
    sput-boolean v1, Lcom/unity3d/player/PlatformSupport;->TIRAMISU_SUPPORT:Z

    const/16 v1, 0x22

    if-lt v0, v1, :cond_6

    const/4 v1, 0x1

    goto :goto_6

    :cond_6
    const/4 v1, 0x0

    :goto_6
    sput-boolean v1, Lcom/unity3d/player/PlatformSupport;->UPSIDE_DOWN_CAKE_SUPPORT:Z

    const/16 v1, 0x23

    if-lt v0, v1, :cond_7

    goto :goto_7

    :cond_7
    const/4 v2, 0x0

    :goto_7
    sput-boolean v2, Lcom/unity3d/player/PlatformSupport;->VANILLA_ICE_CREAM_SUPPORT:Z

    return-void
.end method

.method public constructor <init>()V
    .locals 0

    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    return-void
.end method
