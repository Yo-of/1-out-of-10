import random
import os
import time
import difflib

def is_similar_to_yes(user_input):
    user_input = user_input.lower()
    valid_answer =["yes", "yep", "yup", "eys", "yse", "yeah", "ys", "urq"]

    for answer in valid_answer:
        if difflib.SequenceMatcher(None, user_input, answer).ratio() > 0.6:
            return True
    return False

number = random.randint(0,9)
file = input("Enter a file that you want to play : ")
guess = input("Silly game! Guess the number between 0 and 9 : ")
guess = int(guess)

if guess == number:
    print("You Won! :)")
else:
    print("You lost :( The right anwser was ",number)
    retry = input("Wanna retry ? (Yes/No) : ")
    retry = str(retry)
    if is_similar_to_yes(retry):  
        print("'Kay let's try again")
    else:
        print("How unfortunate... See y'a next time")
    