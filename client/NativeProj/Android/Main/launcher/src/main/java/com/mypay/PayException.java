package com.mypay;

import androidx.annotation.NonNull;

public class PayException extends Exception {
    public String Tag;
    public PayException(String tag){
        Tag = tag;
    }

    @NonNull
    @Override
    public String toString() {
        return "[" + Tag + "]:" + super.toString();
    }
}
