package com.ggxx.nativetools;

import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

public class NativeTools {

    public static void ToastMessage(Context context,  String message, int toastType) {
        Log.d("Unity", "toast Message from Unity: " + message);
        Toast.makeText(context, message, toastType).show();
    }

    public static void RestartApp(Context context, int delay){
        Log.d("Unity", "ForceRestart1: ");
        Intent intent = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName());
        if (intent != null) {
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK | Intent.FLAG_ACTIVITY_NEW_TASK);
            intent.putExtra("REBOOT", "reboot");

            // 使用 Handler 进行延迟重启
            new android.os.Handler().postDelayed(() -> {
                context.startActivity(intent);
                // 杀掉当前进程
                android.os.Process.killProcess(android.os.Process.myPid());
            }, delay); // 延迟时间，以毫秒为单位
        }
    }
}
