package com.knockgame.googlesdk;

import android.app.Activity;
import android.os.Build;
import android.util.Log;

import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.android.play.core.integrity.IntegrityManager;
import com.google.android.play.core.integrity.IntegrityManagerFactory;
import com.google.android.play.core.integrity.IntegrityTokenRequest;
import com.google.android.play.core.integrity.IntegrityTokenResponse;

import java.nio.charset.StandardCharsets;
import java.util.Base64;

public class PlayIntegrityHelper {

    private static final String TAG = "PlayIntegrityHelper";
    private Activity unityActivity;
    private IntegrityCallback integrityCallback; // Java 回调接口

    public PlayIntegrityHelper(Activity activity) {
        this.unityActivity = activity;
    }

    public void setIntegrityCallback(IntegrityCallback callback) {
        this.integrityCallback = callback;
    }

    public void requestIntegrityToken(String nonce) {
        Log.i("PlayIntegrity", "requestIntegrityToken -------1");
        Log.i("PlayIntegrity", "unityActivity ------- 11" + nonce);

        if (integrityCallback == null) {
            Log.e(TAG, "IntegrityCallback is null!");
            return;
        }

        IntegrityManager integrityManager = IntegrityManagerFactory.create(unityActivity);
        IntegrityTokenRequest request = IntegrityTokenRequest.builder()
                .setNonce(nonce)  // 必须设置 nonce
                .build();
        Task<IntegrityTokenResponse> integrityTask = integrityManager.requestIntegrityToken(request);

        integrityTask.addOnSuccessListener(new OnSuccessListener<IntegrityTokenResponse>() {
            @Override
            public void onSuccess(IntegrityTokenResponse response) {
                String integrityToken = response.token();
                Log.d(TAG, "Integrity Token: " + integrityToken);
                integrityCallback.onIntegritySuccess(integrityToken); // 回调成功
            }
        });

        integrityTask.addOnFailureListener(new OnFailureListener() {
            @Override
            public void onFailure(Exception e) {
                Log.e(TAG, "Integrity check failed", e);
                integrityCallback.onIntegrityFailure(e.getMessage()); // 回调失败
            }
        });
    }
}


