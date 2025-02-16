package com.knockgame.googlesdk;

public interface IntegrityCallback {
    void onIntegritySuccess(String integrityToken);
    void onIntegrityFailure(String errorMessage);
}
